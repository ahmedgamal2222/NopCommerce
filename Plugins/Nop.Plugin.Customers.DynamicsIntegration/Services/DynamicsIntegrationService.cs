using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Plugin.Customers.DynamicsIntegration.Services
{
    public class DynamicsIntegrationService
    {
        private readonly ServiceClient _serviceClient;

        public DynamicsIntegrationService()
        {
            string connectionString = "AuthType=ClientSecret;Url=https://ititasks.crm11.dynamics.com/;ClientId=cefdd43b-ca9d-4043-9285-9b9006c48f24;ClientSecret=MVh8Q~MZWuW0IVTDJMq1yJP8npKZiQtraxvsNaEL;Authority=https://login.microsoftonline.com/6960921c-9555-4fa0-ae99-2b5a385deedc;";
            _serviceClient = new ServiceClient(connectionString);
        }


        public bool ContactExists(string email)
        {
            var query = $"<fetch top='1'><entity name='contact'><attribute name='emailaddress1' /><filter><condition attribute='emailaddress1' operator='eq' value='{email}' /></filter></entity></fetch>";
            var results = _serviceClient.RetrieveMultiple(new Microsoft.Xrm.Sdk.Query.FetchExpression(query));
            return results.Entities.Count > 0;
        }

        public void CreateContact(string firstName, string lastName, string email)
        {
            var contact = new Microsoft.Xrm.Sdk.Entity("contact")
            {
                ["firstname"] = firstName,
                ["lastname"] = lastName,
                ["emailaddress1"] = email
            };
            _serviceClient.Create(contact);
        }

        public Microsoft.Xrm.Sdk.Entity GetContactByEmail(string email)
        {
            var query = $"<fetch top='1'><entity name='contact'><attribute name='firstname' /><attribute name='lastname' /><attribute name='emailaddress1' /><filter><condition attribute='emailaddress1' operator='eq' value='{email}' /></filter></entity></fetch>";
            var results = _serviceClient.RetrieveMultiple(new Microsoft.Xrm.Sdk.Query.FetchExpression(query));
            return results.Entities.FirstOrDefault();
        }

        public void UpdateContact(string firstName, string lastName, string email)
        {
            var contact = GetContactByEmail(email);
            if (contact != null)
            {
                contact["firstname"] = firstName;
                contact["lastname"] = lastName;
                _serviceClient.Update(contact);
            }
        }

        public void DeleteContact(string email)
        {
            var contact = GetContactByEmail(email);
            if (contact != null)
            {
                _serviceClient.Delete(contact.LogicalName, contact.Id);
            }
        }

        public List<object> GetAllContacts()
        {
            var query = "<fetch><entity name='contact'><attribute name='firstname' /><attribute name='lastname' /><attribute name='emailaddress1' /></entity></fetch>";
            var results = _serviceClient.RetrieveMultiple(new Microsoft.Xrm.Sdk.Query.FetchExpression(query));

            return results.Entities.Select(e => new
            {
                FirstName = e.GetAttributeValue<string>("firstname"),
                LastName = e.GetAttributeValue<string>("lastname"),
                Email = e.GetAttributeValue<string>("emailaddress1")
            }).ToList<object>();
        }
    }
}
