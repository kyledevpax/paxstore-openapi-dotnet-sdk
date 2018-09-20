﻿using log4net;
using Newtonsoft.Json;
using NUnit.Framework;
using Paxstore.OpenApi;
using Paxstore.OpenApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paxstore.Test
{
    [TestFixture()]
    class TestMerchantApi
    {
        private static ILog _logger = LogManager.GetLogger(typeof(TestMerchantApi));
        public static MerchantApi API = new MerchantApi("https://api.whatspos.cn/p-market-api/", "ZJFXJAG7SJXPPESKVAPO", "AXN5ES2BFYYY8FRMSAPXKQ2ZMT22WYTQGCOGGFM9");

        [Test]
        public void TestSearchMerchantAll() {
            Result<PagedMerchant> result = API.SearchMerchant(1, 10, MerchantSearchOrderBy.Name, null, MerchantStatus.All);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, 0);
        }

        [Test]
        public void TestGetMerchantInvalidId() {
            Result<Merchant> result = API.GetMerchant(0);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, -1);
        }

        [Test]
        public void TestGetMerchantNotExist() {
            Result<Merchant> result = API.GetMerchant(10);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, 1720);
        }

        [Test]
        public void TestCreateMerchantNull() {
            Result<Merchant> result = API.CreateMerchant(null);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, -1);
        }

        [Test]
        public void TestCreateMerchantInvalid() {
            MerchantCreateRequest merchantCreateRequest = new MerchantCreateRequest();
            Result<Merchant> result = API.CreateMerchant(merchantCreateRequest);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, -1);
        }

        [Test]
        public void TestCreateMerchantSuccess()
        {
            MerchantCreateRequest merchantCreateRequest = new MerchantCreateRequest();
            merchantCreateRequest.Name = "KFC";
            merchantCreateRequest.Email = "abc@163.com";
            merchantCreateRequest.ResellerName = "reseller_002";
            merchantCreateRequest.Contact = "tan";
            merchantCreateRequest.Country = "CN";
            merchantCreateRequest.Description = "Merchant KFC";
            merchantCreateRequest.Phone = "23231515";

            Result<Merchant> result = API.CreateMerchant(merchantCreateRequest);
            _logger.DebugFormat("Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, 0);
        }

        [Test]
        public void TestActivateMerchant() {
            long merchantId = 1000000134;
            API.ActivateMerchant(merchantId);
        }

        [Test]
        public void TestCreateUpdateActiveDisableDelete() {
            MerchantCreateRequest merchantCreateRequest = new MerchantCreateRequest();
            merchantCreateRequest.Name = "好人民间";
            merchantCreateRequest.Email = "haoren@163.com";
            merchantCreateRequest.ResellerName = "Pine Labs";
            merchantCreateRequest.Contact = "haoren";
            merchantCreateRequest.Country = "CN";
            merchantCreateRequest.Description = "商户好人民间";
            merchantCreateRequest.Phone = "0512-59564515";
            Result<Merchant> result = API.CreateMerchant(merchantCreateRequest);
            _logger.DebugFormat("Create Merchant Result=\n{0}", JsonConvert.SerializeObject(result));
            Assert.AreEqual(result.BusinessCode, 0);
            long merchantId = result.Data.ID;

            MerchantUpdateRequest merchantUpdateRequest = new MerchantUpdateRequest();
            merchantUpdateRequest.Name = "好人民间";
            merchantUpdateRequest.Email = "haoren2@163.com";
            merchantUpdateRequest.ResellerName = "Pine Labs";
            merchantUpdateRequest.Contact = "haoren2";
            merchantUpdateRequest.Country = "CN";
            merchantUpdateRequest.Description = "商户好人民间";
            merchantUpdateRequest.Phone = "0512-88889999";
            Result<Merchant> updateResult = API.UpdateMerchant(merchantId, merchantUpdateRequest);
            _logger.DebugFormat("Update Merchant Result=\n{0}", JsonConvert.SerializeObject(updateResult));
            Assert.AreEqual(updateResult.BusinessCode, 0);

            Assert.AreEqual(updateResult.Data.Contact, "haoren2");
            Assert.AreEqual(updateResult.Data.Phone, "0512-88889999");
            Assert.AreEqual(updateResult.Data.Email, "haoren2@163.com");

            Result<string> activateResult = API.ActivateMerchant(merchantId);
            _logger.DebugFormat("Activate Merchant Result=\n{0}", JsonConvert.SerializeObject(activateResult));
            Assert.AreEqual(activateResult.BusinessCode, 0);

            Result<string> disableResult = API.DisableMerchant(merchantId);
            _logger.DebugFormat("DisableResult Merchant Result=\n{0}", JsonConvert.SerializeObject(disableResult));
            Assert.AreEqual(disableResult.BusinessCode, 0);

            Result<string> deleteResult = API.DeleteMerchant(merchantId);
            _logger.DebugFormat("Delete Merchant Result=\n{0}", JsonConvert.SerializeObject(deleteResult));
            Assert.AreEqual(deleteResult.BusinessCode, 0);
        }

    }
}