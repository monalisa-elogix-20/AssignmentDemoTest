using AssignmentGit.BusinessRule;
using AssignmentGit.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AssignmentGitTest.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDataFromGitTest()
        {
            ApiOptions options = new ApiOptions
            {
                StartPage = 1,
                PageSize = 100,
                PageCount = 1
            };

            var ghClient = new GitHubClient(new ProductHeaderValue("OctokitTests"), new Uri("https://github.com/octokit/octokit.net.git"));
            var repositoryCommt = ghClient.Repository.Commit.GetAll("octokit", "octokit.net", options).Result;
            var commitMessages = repositoryCommt.Where(x => !string.IsNullOrEmpty(x.Commit.Message)).Select(x => x?.Commit?.Message).ToList();

            Assert.IsNotNull(commitMessages);
        }

        //Binary search test
        [TestMethod]
        public void GetSortedMessageList()
        {
            List<string> commitMessages = new List<string>();
            commitMessages.Add("Hello Hello World");
            commitMessages.Add("Hello Program In Java");
            commitMessages.Add("Hello Java World");
            commitMessages.Add("Hello Hello World Hello Program In Java Hello Java World");


            var bRule = new BinarySortDisplayRule();
            var result = bRule.MakeSortedCommentsByASCIIDictionary(commitMessages);
            var resultSrlz = result.Select(x => x.Value).Select(x => string.Join(" ", x.Select(v => v.Word)));

            Assert.IsNotNull(resultSrlz);
        }

        //Word Count test
        [TestMethod]
        public void GetWordOccurrencesCount()
        {
            List<string> commitMessages = new List<string>();
            commitMessages.Add("Hello Hello World");
            commitMessages.Add("Hello Program In Java");
            commitMessages.Add("Hello Java World");
            commitMessages.Add("Hello Hello World Hello Program In Java Hello Java World");


            var occourencesCountHelper = new OccourencesCountRule();
            var wordCountData = occourencesCountHelper.GetAllWordsCount(commitMessages) ?? new Dictionary<string, int>();

            Assert.IsNotNull(wordCountData);
        }

        [TestMethod]
        public void GitTest2()
        {
            var probe = new EnterpriseProbe(new ProductHeaderValue("my-cool-app"));
            var result = probe.Probe(new Uri("http://ghe.example.com/"));
            Assert.AreEqual(EnterpriseProbeResult.Ok, result);
        }

        [TestMethod]
        public void GitTest3()
        {
            var ghe = new Uri("https://github.myenterprise.com/");
            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"), ghe);
            //var user = client.User.Get("shiftkey");
            var user = client.User.Current();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GitTest4()
        {
            var ghe = new Uri("https://github.com/octokit/octokit.net.git");
            var client = new GitHubClient(new ProductHeaderValue("OctokitTests"), ghe);
            client.Credentials = new Credentials("");
            ApiOptions options = new ApiOptions
            {
                StartPage = 1,
                PageSize = 10,
                PageCount = 1
            };
            var repositoryCommt = client.Repository.Commit.GetAll("octokit", "octokit.net", options).Result;
            //var repositoryCommt = client.Repository.Status.GetAll("octokit", "octokit.net", options).Result;
            var user = client.User.Current();
            Assert.IsNotNull(user);
        }
    }
}
