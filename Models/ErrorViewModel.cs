/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- This model represents the error view in the PcPulse application.
- It contains a property RequestId, which holds the unique identifier of the request causing the error.
- The RequestId property is nullable (?), allowing it to be null if no request ID is provided.
- The ShowRequestId property returns true if the RequestId property is not null or empty, indicating that the request ID should be shown.
*/

namespace PcPulse.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
