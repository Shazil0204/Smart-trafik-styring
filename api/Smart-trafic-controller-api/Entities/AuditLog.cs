using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Entities
{
    public class AuditLog
    {
        public int Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string OperationType { get; private set; } = null!;
        public string EntityName { get; private set; } = null!;
        public string EntityId { get; private set; } = null!;

        private AuditLog() { } // For EF Core
        
        public AuditLog(string operationType, string entityName, string entityId)
        {
            Timestamp = DateTime.UtcNow;
            OperationType = operationType;
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}