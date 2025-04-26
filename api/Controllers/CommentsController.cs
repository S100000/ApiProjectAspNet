using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dto.CommentsDto;

namespace api.Controllers
{
    
    [Route("Api/Comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IComment _commentRepo;
        private readonly IStock _stockRepo;
        public CommentsController(IComment commentRepo, IStock stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comment = await _commentRepo.GetAllAsync();
            var commentDto = comment.Select(a => a.ToCommentDto());

            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CommentToCreateDto commentDto)
        {
            if(!await _stockRepo.CheckStock(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCreateComment(stockId);
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new{ id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
        {
            var commentModel = await _commentRepo.UpdateAsync(id, commentDto);
            if(commentModel == null)
            {
                return NotFound();
            }

            return Ok(commentModel.ToCommentDto());
        }
    }
}