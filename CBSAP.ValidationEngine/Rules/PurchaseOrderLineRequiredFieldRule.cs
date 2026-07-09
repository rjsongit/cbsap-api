using CbsAp.Domain.Entities.DimensionSetup;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;
using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Rules
{
    public class PurchaseOrderLineRequiredFieldRule : IValidationRule
    {
        public string Field { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineValidationSeverity Severity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceStatusType? NextStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EngineInvoiceQueueType? TargetQueue { get; set; }

        public string Name => "PurchaseOrderLineRequiredFieldRule";

        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var type = context.GetType();
            var prop = type.GetProperty(Field);
            var value = prop?.GetValue(context);

            var isMissing = value == null || (value is string s && string.IsNullOrWhiteSpace(s));

            // Extra: Ensure SupplierID exists in SupplierInfos from context

            var hasInvoiceAllocationLines = runtimeContext!.TryGetValue("InvoiceAllocationLines", out var invoiceAllocationLinesObj);
            var hasDimensionSetups = runtimeContext!.TryGetValue("DimensionSetup", out var dimensionSetupObj);

            if (hasInvoiceAllocationLines && hasDimensionSetups)
            {
                var dimensionSetups = dimensionSetupObj as IEnumerable<DimensionSetup>;
                var invoiceAllocationLines = invoiceAllocationLinesObj as IEnumerable<InvAllocLine>;
                var requiredFields = dimensionSetups.Where(x => x.Required == true && x.Show == true).Select(x => x.DimensionSetupName.Replace(" ","").ToUpper()).ToList();
                var errorFields = new List<string>();
                if (invoiceAllocationLines.Count() > 0) {

                   // if has required value in dimensions and has po allocationline make it sure validate the trigger

                    foreach (var inAllocLine in invoiceAllocationLines)
                    {

                        if(inAllocLine.Dimensions.Count() > 0)
                        {
                            errorFields = inAllocLine.Dimensions.Where(x => requiredFields.Contains(x.DimensionKey.Replace(" ", "").ToUpper()) && (x.DimensionValue == "0" || string.IsNullOrEmpty(x.DimensionValue) || x.DimensionValue.ToUpper() == "NAN")).Select(x => x.DimensionKey.ToUpper()).ToList();
                        }
                        else
                        {
                            errorFields = dimensionSetups.Where(x => x.Required == true && x.Show == true).Select(x => x.DimensionSetupName.Replace(" ","").ToUpper()).ToList();
                        }

                        
                    }

                    errorFields = dimensionSetups.Where(x => errorFields.Contains(x.DimensionSetupName.Replace(" ", "").ToUpper())).Select(x => x.DimensionName).ToList();
                }
        
                if(errorFields.Count() > 0)
                {
                    ErrorMessage = "Invoice must be coded before being approved: " + string.Join(",", errorFields) + ".";




                    return EngineValidationResult.Failure(
                          ErrorMessage,
                          ErrorCode,
                          Severity,
                          NextStatus,
                          TargetQueue);
                }

            }

            return EngineValidationResult.Success();
        }
    }
}