using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands
{
    public class UpdateAllocationLineCommandHandler : ICommandHandler<UpdateAllocationLineCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public UpdateAllocationLineCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateAllocationLineCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitofWork.GetRepository<InvAllocLine>();
            var dto = request.invoiceDto;
            var invAllocLine = await repo.GetByIdAsync(dto.InvAllocLineID);
            if (invAllocLine == null)
            {
                return ResponseResult<bool>.NotFound("Allocation Line not found.");

            }
            //Need this for Entity Audit Logs to preserve the previous data
            foreach (var dimension in invAllocLine.Dimensions)
            {
                var existedRecord = dto.Dimensions
                    .FirstOrDefault(x => x.InvAllocLineDimensionID == dimension.InvAllocLineDimensionID);

                if (existedRecord != null)
                {
                    dimension.DimensionKey = existedRecord.DimensionKey;
                    dimension.DimensionValue = existedRecord.DimensionValue;
                }
            }

            //get new items
            var existingIds = invAllocLine.Dimensions.Select(d => d.InvAllocLineDimensionID).ToList();

            var newItems = dto.Dimensions
                .Where(x => !existingIds.Contains(x.InvAllocLineDimensionID))
                .ToList();

            foreach (var newItem in newItems)
            {
                invAllocLine.Dimensions.Add(new InvAllocLineDimension
                {
                    InvAllocLineID = invAllocLine.InvAllocLineID,
                    DimensionKey = newItem.DimensionKey,
                    DimensionValue = newItem.DimensionValue
                });
            }




            invAllocLine.InvAllocLineID = dto.InvAllocLineID;
            invAllocLine.InvoiceID = dto.InvoiceID;
            invAllocLine.LineNo = dto.LineNo;
            invAllocLine.PoNo = dto.PoNo;
            invAllocLine.PoLineNo = dto.PoLineNo;
            invAllocLine.Qty = dto.Qty;
            invAllocLine.LineDescription = dto.LineDescription;
            invAllocLine.Note = dto.Note;
            invAllocLine.LineNetAmount = dto.LineNetAmount;
            invAllocLine.LineTaxAmount = dto.LineTaxAmount;
            invAllocLine.LineAmount = dto.LineAmount;
            invAllocLine.TaxCodeID = dto.TaxCodeID;
            invAllocLine.AccountID = dto.Account;

            invAllocLine.SetAuditFieldsOnUpdate(request.UpdatedBy);
      

            var isSaved = await _unitofWork.SaveChanges(request.UpdatedBy, "InvoiceAllocation", cancellationToken);
            if (!isSaved)
            {
                return ResponseResult<bool>.BadRequest("Failed to update Allocation Line ");
            }

            return ResponseResult<bool>.OK("Allocation Line updated successfully.");
        }

      
    }
}