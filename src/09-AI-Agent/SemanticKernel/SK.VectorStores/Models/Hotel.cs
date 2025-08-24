﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.VectorData;

namespace SK.VectorStores.Models
{

    public static class EmbeddingDims
    {
        public const int Description = 1024;
    }
    public class Hotel
    {
        [VectorStoreKey(StorageName = "hotel_id")]
        public int HotelId { get; set; }

        [VectorStoreData(StorageName = "hotel_name")]
        public string? HotelName { get; set; }

        [VectorStoreData(StorageName = "hotel_description")]
        public string? Description { get; set; }

        [VectorStoreVector(Dimensions: EmbeddingDims.Description, DistanceFunction = DistanceFunction.CosineSimilarity, StorageName = "description_embedding")]
        public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }
    }
}