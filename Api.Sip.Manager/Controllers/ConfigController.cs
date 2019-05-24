using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Sip.Manager.Models;
using Api.Sip.Manager.Components;

namespace Api.Sip.Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConfigController : ControllerBase
    {
        private readonly GitHubService _gitHubService;

        public ConfigController(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        // GET: api/config
        [HttpGet]
        public async Task<IEnumerable<ContentModel>> Get()
        {
            var content = await _gitHubService.GetContent();
            
            return content.Where(x => x.name.StartsWith("SEP") && x.name.EndsWith(".cnf.xml"));
        }

        // GET: api/config/filename.xml
        [HttpGet("{fileName}")]
        public async Task<ContentModel> Get(string fileName)
        {
            return await _gitHubService.GetContent(fileName);
        }

        // GET: api/config/filename.xml/content
        [HttpGet("{name}/content", Name = "GetContent")]
        public async Task<string> GetContent(string name)
        {
            return await _gitHubService.GetConfigurationFromContent(name);
        }

        // GET: api/config/filename.xml/config
        [HttpGet("{name}/config", Name = "GetConfig")]
        public async Task<ConfigModel> GetConfig(string name)
        {
            //need to return an error at some point
            if (!name.EndsWith(".xml")) { return null; }

            var content = await _gitHubService.GetConfigurationFromContent(name);
            var xml = XDocument.Parse(content);
            var model = new ConfigModel();

            try
            {
                var xmlLine = xml.Root
                                .Element("sipProfile")
                                .Element("sipLines")
                                .Element("line");

                model.featureLabel = xmlLine.Element(nameof(model.featureLabel)).Value;
                model.name = xmlLine.Element(nameof(model.name)).Value;
                model.displayName = xmlLine.Element(nameof(model.displayName)).Value;
                model.contact = xmlLine.Element(nameof(model.contact)).Value;
                model.authName = xmlLine.Element(nameof(model.authName)).Value;
                model.authPassword = xmlLine.Element(nameof(model.authPassword)).Value;

                return model;
            }
            catch (XmlException ex)
            {
                //need to return an error at some point
                return null;
            }

        }

        // POST: api/config/filename.xml
        [HttpPost("{fileName}")]
        public async Task<string> Post(string fileName, [FromBody] ConfigModel model)
        {
            //need to return an error at some point
            if (fileName == null || !fileName.EndsWith(".xml")) { return null; }
            if (model == null) { return null; }

            var content = await GetAndSetFileContent(fileName, model, false);

            var response = await _gitHubService.CreateContent(
                fileName,
                Models.Create.ContentFileModel.GetModel(User, content, $"Initializing phone to {model.name}.")
            );

            return response;
        }

        // PUT: api/config/filename.xml
        [HttpPut("{fileName}")]
        public async Task<string> PutConfig(string fileName, [FromBody] ConfigModel model)
        {
            //need to return an error at some point
            if (fileName == null || !fileName.EndsWith(".xml")) { return null; }
            if (model == null) { return null; }

            var content = await GetAndSetFileContent(fileName, model, true);

            var contentModel = await Get(fileName);

            var response = await _gitHubService.UpdateContent(
                fileName,
                Models.Update.ContentFileModel.GetModel(User, content, $"Reprovisioning phone for {model.name}.", contentModel.sha)
            );

            return response;
        }

        // DELETE: api/config/filename.xml
        [HttpDelete("{name}")]
        public void Delete(string name)
        {

        }

        private async Task<string> GetAndSetFileContent(string fileName, ConfigModel model, bool isExistingFile)
        {
            //Get a template file from GitHub
            var content = await _gitHubService.GetConfigurationFromContent(isExistingFile ? fileName : "template.cnf.xml");
            var xml = XDocument.Parse(content);

            //Mutate it
            if (!ConfigService.ApplyModelToXML(model, xml)) { return null; /*should return error on false at some point*/ }

            return xml.ToString();
        }
    }
}
