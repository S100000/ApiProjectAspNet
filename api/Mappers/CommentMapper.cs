using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.CommentsDto;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comments comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId
            };
        }

        public static Comments ToCreateComment(this CommentToCreateDto commentDto, int sotckId)
        {
            return new Comments
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = sotckId
            };
        }
    }
}