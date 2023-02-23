﻿using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.Estimations.Productions.DataTransferObjects
{
    public class UpdateEstimatedProductionDetailDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "OrderId")]
        public Guid OrderId { get; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "TotalGram")]
        public double TotalGram { get; }

        [JsonProperty(PropertyName = "TotalOrder")]
        public double TotalOrder { get; }

        [JsonProperty(PropertyName = "GradeA")]
        public double GradeA { get; private set; }

        [JsonProperty(PropertyName = "GradeB")]
        public double GradeB { get; private set; }

        [JsonProperty(PropertyName = "GradeC")]
        public double GradeC { get; private set; }

        [JsonProperty(PropertyName = "GradeD")]
        public double GradeD { get; private set; }

        public UpdateEstimatedProductionDetailDto(OrderDocument orderDocument, FabricConstructionDocument fabricConstructionDocument, EstimatedProductionDetail estimatedDetail)
        {
            Id = estimatedDetail.Identity;
            OrderId = estimatedDetail.OrderId.Value;
            DateOrdered = orderDocument.AuditTrail.CreatedDate;
            OrderNumber = orderDocument.OrderNumber;
            TotalOrder = orderDocument.AllGrade;
            ConstructionNumber = fabricConstructionDocument.ConstructionNumber;
            TotalGram = fabricConstructionDocument.TotalYarn;
            GradeA = estimatedDetail.GradeA;
            GradeB = estimatedDetail.GradeB;
            GradeC = estimatedDetail.GradeC;
            GradeD = estimatedDetail.GradeD ?? 0;
        }
    }
}
