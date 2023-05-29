using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Agenda.Server.Controllers
{
    public class ExportarController : Controller
    {

        public FileStreamResult ToXML(
            IQueryable query, 
            string fileName = null)
        {
            var elementType = query.ElementType;
            var properties = elementType.GetProperties();
            var columns = properties.Select(p => p.Name).ToList();
            var stream = new MemoryStream();

            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Root");

                foreach (var item in query)
                {
                    writer.WriteStartElement("Item");

                    foreach (var columnName in columns)
                    {
                        if(columnName != "Item")
                        {
                            var property = elementType.GetProperty(columnName);
                            var value = property?.GetValue(item);
                            var stringValue = $"{value}".Trim();

                            var propertyName = GetPropertyName(property);
                            writer.WriteElementString(propertyName, stringValue);
                        }
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            if (stream?.Length > 0)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            var result = new FileStreamResult(stream, "application/xml");
            result.FileDownloadName = (!string.IsNullOrEmpty(fileName) ? fileName : "Export") + ".xml";

            return result;
        }

        public IQueryable AplicarQuery<T>(
            IQueryable<T> items, 
            IQueryCollection query = null) where T : class
        {
            if (query != null)
            {
                if (query.ContainsKey("$expand"))
                {
                    var propertiesToExpand = query["$expand"].ToString().Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                var filter = query.ContainsKey("$filter") 
                    ? query["$filter"].ToString() 
                    : null;
                if (!string.IsNullOrEmpty(filter))
                {
                    items = items.Where(filter);
                }

                if (query.ContainsKey("$orderBy"))
                {
                    items = items.OrderBy(query["$orderBy"].ToString());
                }

                if (query.ContainsKey("$skip"))
                {
                    items = items.Skip(int.Parse(query["$skip"].ToString()));
                }

                if (query.ContainsKey("$top"))
                {
                    items = items.Take(int.Parse(query["$top"].ToString()));
                }

                if (query.ContainsKey("$select"))
                {
                    return items.Select($"new ({query["$select"].ToString()})");
                }
            }

            return items;
        }


        private string GetPropertyName(
            PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute(typeof(XmlElementAttribute)) as XmlElementAttribute;
            return attribute?.ElementName ?? property.Name;
        }
    }
}
