# SipPhoneManager

Three projects here:
1. Spa.Sip.Manager is a single page application written in Angular6. It is the real goal of the project, to give our administartor the ability to provision and re-configure existing SIP phones used by employees.
2. Api.Sip.Manager is a RESTful API which encapsulates the integration with GitHub's API needed to store our config files for each phone. Those files are in turn deployed by GitHub to a TFTP server for devices to read from.
3. IdentityServer is an Identity Server 4 implementation and is used to authenticate sessions between the SPA and API. It currently has no local user store and integrates with Google SSO via it's API.

Note: Cloning this repo won't include essential settings needed to run the API and IdentityServer4. To run it the project will need a certificate file used to sign tokens as well as a serversettings.json for both API and IdentityServer containing client IDs and Secret codes. The SPA, however, should run just fine but likely won't be able to authenticate due to CORS
