using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Sip.Manager.Components;

namespace Api.Sip.Manager.Models.Create
{
    public class ContentFileModel
    {
        /// <summary>
        /// Commit Message.
        /// </summary>
        public string message;

        /// <summary>
        /// Serialized in base64.
        /// </summary>
        public string content;

        /// <summary>
        /// Name of the branch to commit to, i.e. master.
        /// </summary>
        public string branch;

        /// <summary>
        /// User information for the one committing the change.
        /// </summary>
        public UserInfoModel committer;

        /// <summary>
        /// User information for the one who created this file.
        /// </summary>
        public UserInfoModel author;

        public static ContentFileModel GetModel(System.Security.Claims.ClaimsPrincipal user, string content, string commitMessage)
        {
            var userName = user?.FindFirst(x => x.Type.Equals(IdentityModel.JwtClaimTypes.Name))?.Value ?? "Alan Sellers";
            var userEmail = user?.FindFirst(x => x.Type.Equals(IdentityModel.JwtClaimTypes.Email))?.Value ?? "baalansellers@hotmail.com";

            return new ContentFileModel()
            {
                message = commitMessage,
                content = Utils.EncodeBase64(content),
                branch = "master",
                committer = new UserInfoModel()
                {
                    name = userName,
                    email = userEmail
                },
                author = new UserInfoModel()
                {
                    name = userName,
                    email = userEmail
                }
            };
        }
    }
}
