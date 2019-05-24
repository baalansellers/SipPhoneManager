using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Sip.Manager.Models.Update
{
    public class ContentFileModel : Create.ContentFileModel 
    {
        public string sha;

        public static ContentFileModel GetModel(System.Security.Claims.ClaimsPrincipal user, string content, string commitMessage, string sha)
        {
            var baseObj = GetModel(user, content, commitMessage);

            return  new ContentFileModel()
            {
                message = baseObj.message,
                content = baseObj.content,
                branch = baseObj.branch,
                committer = baseObj.committer,
                author = baseObj.author,
                sha = sha
            };
        }
    }
}
