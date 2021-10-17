using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace EligereES.Controllers
{
    [AllowAnonymous]
    [Route("Metadata")]
    public class MetadataController : Controller
    {
        private readonly Saml2Configuration config;

        public MetadataController(IOptions<Saml2Configuration> configAccessor)
        {
            config = configAccessor.Value;
        }

        public IActionResult Index()
        {
//            var defaultSite = new Uri($"{Request.Scheme}://{Request.Host.ToUriComponent()}/");
            var defaultSite = new Uri($"https://eligere.unical.it/");

            var entityDescriptor = new EntityDescriptor(config);
            entityDescriptor.ValidUntil = 365;
            entityDescriptor.SPSsoDescriptor = new SPSsoDescriptor
            {
                WantAssertionsSigned = true,
                SigningCertificates = new X509Certificate2[]
                {
                    config.SigningCertificate
                },
                //EncryptionCertificates = new X509Certificate2[]
                //{
                //    config.DecryptionCertificate
                //},
                SingleLogoutServices = new SingleLogoutService[]
                {
                    new SingleLogoutService { Binding = ProtocolBindings.HttpPost, Location = new Uri(defaultSite, "Auth/SingleLogout"), ResponseLocation = new Uri(defaultSite, "Auth/LoggedOut") }
                },
                NameIDFormats = new Uri[] { NameIdentifierFormats.X509SubjectName },
                AssertionConsumerServices = new AssertionConsumerService[]
                {
                    new AssertionConsumerService { Binding = ProtocolBindings.HttpPost, Location = new Uri(defaultSite, "Auth/AssertionConsumerService") },
                    new AssertionConsumerService { Binding = ProtocolBindings.HttpPost, Location = new Uri(defaultSite, "Auth/AssertionConsumerService-test"), IsDefault = false }
                },
                AttributeConsumingServices = new AttributeConsumingService[]
                {
                    new AttributeConsumingService { ServiceName = new ServiceName("EligereSP", "en"), RequestedAttributes = CreateRequestedAttributes() }
                },
            };
            entityDescriptor.ContactPersons = new[] { 
                new ContactPerson(ContactTypes.Administrative)
                {
                    Company = "Università della Calabria",
                    GivenName = "Giuseppe",
                    SurName = "De Marco",
                    EmailAddress = "mailto:idem-support@unical.it",
                },
                new ContactPerson(ContactTypes.Technical)
                {
                    Company = "Università della Calabria",
                    GivenName = "Francesco",
                    SurName = "Filicetti",
                    EmailAddress = "mailto:idem-support@unical.it",
                }
            };
            return new Saml2Metadata(entityDescriptor).CreateMetadata().ToActionResult();
        }

        private IEnumerable<RequestedAttribute> CreateRequestedAttributes()
        {
            yield return new RequestedAttribute("codice_fiscale");
            yield return new RequestedAttribute("matricola_dipendente");
            yield return new RequestedAttribute("sn");
            yield return new RequestedAttribute("cn");
        }
    }
}