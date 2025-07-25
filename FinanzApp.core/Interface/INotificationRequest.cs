using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzApp.core.Interface
{
    public interface INotificationRequest
    {
        event EventHandler<string>? NotificationRequested;
    }
}
