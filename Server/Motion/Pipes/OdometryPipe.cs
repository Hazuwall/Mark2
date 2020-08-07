using Common.Messaging;
using Server.Motion.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Motion.Pipes
{
    public class OdometryPipe : IPipe
    {
        private readonly IOdometryProvider _odometry;
        private readonly IKinematicsService _kinematics;

        public OdometryPipe(IOdometryProvider odometry, IKinematicsService kinematics)
        {
            _odometry = odometry;
            _kinematics = kinematics;
        }

        public Transaction Process(Transaction transaction)
        {
            if(transaction.TransactionType == TransactionType.Query)
            {
                var queries = transaction.Messages;
                var response = transaction.Response;
                for (int i = 0; i < queries.Count; i++)
                {
                    try
                    {
                        object data = null;
                        switch (queries[i].MessageType)
                        {
                            case MessageTypes.Queries.Coords:
                                data = _odometry.GetCoords();
                                break;
                            case MessageTypes.Queries.Velocities:
                                data = _odometry.GetVelocities();
                                break;
                            case MessageTypes.Queries.AbsCoords:
                                var coords = _odometry.GetCoords();
                                data = _kinematics.ForwardPosTransform(coords);
                                break;
                            case MessageTypes.Queries.AbsVelocities:
                                var velocities = _odometry.GetCoords();
                                data = _kinematics.ForwardVelocityTransform(velocities);
                                break;
                            default:
                                break;
                        }
                        if (data != null)
                        {
                            response[i] = new Message(queries[i].MessageType, data);
                        }
                    }
                    catch(Exception ex)
                    {
                        response[i] = new Message(MessageTypes.Errors.Calculation, ex.Message);
                    }
                }
            }
            return transaction;
        }
    }
}
