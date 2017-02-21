﻿namespace HomeHub.Hub
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

        [UriFormat("/thermostat/setpollingtime")]
        public IPutResponse SetPollingTime([FromContent]int pollingTime)
        {
            Debug.WriteLine("SetPollingTime");

            try
            {
                Thermostat.Instance.PollingTime = pollingTime;
                return new PutResponse(PutResponse.ResponseStatus.OK);
            }
            catch (Exception e)
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound, e.Message);
            }
        }

        [UriFormat("/thermostat/settargetbuffertime")]
        public IPutResponse SetTargetBufferTime([FromContent]int targetBufferTime)
        {
            Debug.WriteLine("SetTargetBufferTime");

            try
            {
                Thermostat.Instance.TargetBufferTime = targetBufferTime;
                return new PutResponse(PutResponse.ResponseStatus.OK);
            }
            catch (Exception e)
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound, e.Message);
            }
        }

        [UriFormat("/thermostat/setuserules")]
        public IPutResponse SetUseRules([FromContent]bool useRules)
        {
            Debug.WriteLine("SetUseRules");

            try
            {
                Thermostat.Instance.UseRules = useRules;
                return new PutResponse(PutResponse.ResponseStatus.OK);
            }
            catch (Exception e)
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound, e.Message);
            }
        }

        [UriFormat("/thermostat/addrule")]
        public IPutResponse AddRule([FromContent]ScheduleRule rule)
        {
            Debug.WriteLine("AddRule");

            if (rule == null)
            {
                return new PutResponse(PutResponse.ResponseStatus.NoContent);
            }

            try
            {
                Thermostat.Instance.AddRule(rule);
                return new PutResponse(PutResponse.ResponseStatus.OK);
            }
            catch (Exception e)
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound, e.Message);
            }
        }

        [UriFormat("/thermostat/updaterule")]
        public IPostResponse UpdateRule([FromContent]ScheduleRule rule)
        {
            Debug.WriteLine("UpdateRule");

            try
            {
                Thermostat.Instance.UpdateRule(rule);
                return new PostResponse(PostResponse.ResponseStatus.Created);
            }
            catch (Exception e)
            {
                return new PostResponse(PostResponse.ResponseStatus.Conflict, e.Message);
            }
        }

        [UriFormat("/thermostat/deleterule/{id}")]
        public IDeleteResponse DeleteRule(string id)
        {
            Debug.WriteLine("DeleteRule");

            if (String.IsNullOrEmpty(id))
            {
                return new DeleteResponse(DeleteResponse.ResponseStatus.NoContent);
            }

            bool status = Thermostat.Instance.DeleteRule(id);
            return new DeleteResponse(
                status ? DeleteResponse.ResponseStatus.OK : DeleteResponse.ResponseStatus.NotFound);
        }

        [UriFormat("/thermostat/setholdtemp?expiration={expiration}")]
        public IPutResponse SetHoldTemp(DateTime expiration, [FromContent]TemporaryOverrideRule rule)
        {
            Debug.WriteLine("SetHoldTemp");

            if (rule == null)
            {
                return new PutResponse(PutResponse.ResponseStatus.NoContent);
            }

            try
            {
                rule.Expiration = expiration;
                Thermostat.Instance.UpdateRule(rule);
                return new PutResponse(PutResponse.ResponseStatus.OK);
            }
            catch (Exception e)
            {
                return new PutResponse(PutResponse.ResponseStatus.NotFound, e.Message);
            }
        }
    }
}
