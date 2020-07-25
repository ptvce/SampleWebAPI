using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Data;
using SampleWebAPI.Entites;

namespace SampleWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class CustomerController : ControllerBase
    {
        private AdventureWorkSampleDBContext _context;
        public CustomerController(AdventureWorkSampleDBContext context)
        {
            _context = context;
        }

        //http://localhost/api/customer
        /// <summary>
        /// لیست مشتریان سازمان
        /// </summary>
        /// <param name="page">صفحه جاری</param>
        /// <param name="limit">تعداد در هر صفحه</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(int page =1, int limit = 10)
        {
            int skip = (page - 1) * limit;
            var result = _context.Customer.OrderBy(q => q.CustomerId).Skip(skip).Take(limit).ToListAsync();
            return Ok(result);
        }
        /// <summary>
        /// اطلاعات مشتریان بر اساس کد مشتری
        /// </summary>
        /// <param name="id">کد مشتری</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = _context.Customer.Find(id);
            return Ok(result);
        }

        /// <summary>
        /// افزودن مشتری جدید
        /// </summary>
        /// <param name="model">اطلاعات مشتری</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Customer model)    //FromBody : باعث می شود که به جز بادی از روش های دیگر نتواند دیتا ارسال کند مثلا با کوئری استرینگ نتواند
        {
            if (!ModelState.IsValid)
                return BadRequest("اطلاعات ارسالی صحیح نمی باشد");
            await _context.Customer.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Customer model)
        {
            _context.Attach(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           var item = await _context.Customer.FindAsync(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
