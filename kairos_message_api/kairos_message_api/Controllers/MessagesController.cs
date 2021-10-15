using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kairos_message_api.Data;
using kairos_message_api.Models;


using System.Globalization;


using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;


namespace kairos_message_api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {


        private readonly ApplicationDbContext _context;

        IBackgroundJobClient backgroundJobs = new BackgroundJobClient();


        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Messages

        
        [HttpGet]
        public IEnumerable<Message> GetMessage()
        {
            return _context.Message;
        }

        


       
        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = await _context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
        
        

        // PUT: api/Messages/5

        

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage([FromRoute] int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.message_id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        



        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Cast
            
            int c_message_id = Convert.ToInt32(message.message_id);
            Guid c_message_unique_id = message.message_unique_id;
            int c_message_job_type = Convert.ToInt32(message.message_job_type);

            DateTime c_time_for_delivery_utc = message.time_for_delivery_utc;

            

            //current time
            //long unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            // send in 10 secs
            //long presettest = unixSeconds + 10;


            
            int c_client_id = Convert.ToInt32(message.client_id);
            string c_client_name = message.client_name.ToString(); 
            string c_message_body = message.message_body.ToString(); 
            string c_sms_phone_no = message.sms_phone_no.ToString();
            

            backgroundJobs.Enqueue<sms_message>(x => x.Send(c_message_id, c_message_unique_id, c_message_job_type, c_time_for_delivery_utc, c_client_id, c_client_name, c_message_body, c_sms_phone_no));


            //backgroundJobs.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromSeconds(30));
            //backgroundJobs.Enqueue(() => Console.WriteLine());

            //backgroundJobs.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromSeconds(30));

            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.message_id }, message);
        }

        // DELETE: api/Messages/5

    

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

       

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.message_id == id);
        }
    }
}