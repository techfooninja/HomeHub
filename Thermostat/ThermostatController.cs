namespace HomeHub.Hub
{
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Contracts;
    using Restup.Webserver.Models.Schemas;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    [RestController(InstanceCreationType.Singleton)]
    class ThermostatController
    {
        // TODO: Functionality
        //  Current Temperature
        //  Set Temperature
        //  Support rules and hold temperature

        [UriFormat("/thermostat")]
        public IGetResponse GetStatus()
        {
            Debug.WriteLine("GetStatus");
            return new GetResponse(
              GetResponse.ResponseStatus.OK,
              Thermostat.Instance);
        }

        [UriFormat("/thermostat/addrule")]
        public IPutResponse AddRule([FromContent]ScheduleRule rule)
        {
            Debug.WriteLine("AddRule");

            Thermostat.Instance.AddRule(rule);

            return new PutResponse(
                PutResponse.ResponseStatus.OK);
        }

        [UriFormat("/thermostat/updaterule")]
        public IPostResponse UpdateRule([FromContent]ScheduleRule rule)
        {
            Debug.WriteLine("UpdateRule");

            Thermostat.Instance.UpdateRule(rule);

            return new PostResponse(
                PostResponse.ResponseStatus.Created);
        }

        [UriFormat("/thermostat/deleterule/{id}")]
        public IDeleteResponse DeleteRule(string id)
        {
            Debug.WriteLine("DeleteRule");

            bool status = Thermostat.Instance.DeleteRule(id);

            return new DeleteResponse(
                status ? DeleteResponse.ResponseStatus.OK : DeleteResponse.ResponseStatus.NotFound);
        }

        [UriFormat("/thermostat/setholdtemp?expiration={expiration}")]
        public IPutResponse SetHoldTemp(DateTime expiration, [FromContent]TemporaryOverrideRule rule)
        {
            Debug.WriteLine("SetHoldTemp");

            rule.Expiration = expiration;

            Thermostat.Instance.UpdateRule(rule);

            return new PutResponse(
                PutResponse.ResponseStatus.OK);
        }
    }
}
