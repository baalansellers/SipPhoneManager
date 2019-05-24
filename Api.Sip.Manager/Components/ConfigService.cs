using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Threading.Tasks;
using Api.Sip.Manager.Models;

namespace Api.Sip.Manager.Components
{
    public class ConfigService
    {
        public static bool ApplyModelToXML(ConfigModel configModel, XDocument xmlDoc)
        {
            try
            {
                var xmlLine = xmlDoc.Root
                                .Element("sipProfile")
                                .Element("sipLines")
                                .Element("line");

                xmlLine.Element(nameof(ConfigModel.featureLabel)).Value = configModel.featureLabel;
                xmlLine.Element(nameof(ConfigModel.name)).Value = configModel.name;
                xmlLine.Element(nameof(ConfigModel.displayName)).Value = configModel.displayName;
                xmlLine.Element(nameof(ConfigModel.contact)).Value = configModel.contact;
                xmlLine.Element(nameof(ConfigModel.authName)).Value = configModel.authName;
                xmlLine.Element(nameof(ConfigModel.authPassword)).Value = configModel.authPassword;

            }
            catch (XmlException ex)
            {
                //need to return an error at some point
                return false;
            }

            return true;
        }
    }
}
