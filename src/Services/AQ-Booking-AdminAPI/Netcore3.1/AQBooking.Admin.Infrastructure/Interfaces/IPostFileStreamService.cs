using APIHelpers.Response;
using AQBooking.Admin.Core.Models.PostFileStream;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPostFileStreamService
    {
        PostFileStreamViewModel GetPostFileStreamById(long id);
        List<PostFileStreamViewModel> GetPostFileStreamByPostId(int postId);

        long CreatePostFileStream(PostFileStreamCreateModel model);

        long UpdatePostFileStream(PostFileStreamCreateModel model);

        bool DeletePostFileStream(int id);
    }
}
