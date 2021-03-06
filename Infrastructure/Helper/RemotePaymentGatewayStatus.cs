using Domain.Entities.Enums;
using System;

namespace Infrastructure.Helper
{
    public static class RemotePaymentGatewayStatus
    {
        /*
             * for purpose of simulation, since we are not connecting to a payment gateway
             * Generate a randon number from 0 - 2
             * 0 - Means the payment action failed
             * 1 - Means the payment action is processed
             * 2 - Means the payment action is pending
             */
        public static PayState Process()
        {
            var rand = new Random();
            var result = rand.Next(2);
            if (result == 0)
            {
                return PayState.Failed;
            }
            else if (result == 1)
            {
                return PayState.Processed;
            }
            else
            {
                return PayState.Pending;
            }
        }
    }
}
