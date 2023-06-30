using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Net.Http.Headers;
using System.IO;
using System.Net;

namespace gitlibrary
{
    internal class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();
        static async Task<string>  GetAccessToken()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                // Replace "yourUsername" and "yourPassword" with your GitHub credentials
                string username = "anilkumara";
                string password = "Anil1@34";
                string responseJson = "";
                // Set the desired note and scopes for the token
                string note = "Token generated programmatically";
                string scopes = "repo,user";

                // Prepare the JSON payload for the request
                string payload = $"{{\"note\":\"{note}\", \"scopes\":[{scopes}]}}";

                // Create the request to the GitHub API
                string apiUrl = "https://api.github.com/authorizations";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}")));
                request.ContentType = "application/json";

                // Write the JSON payload to the request body
                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(payload);
                }

                // Send the request and retrieve the response
                using (HttpWebResponse response =  (HttpWebResponse)request.GetResponse())
                {
                    // Read the response body
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseJson = reader.ReadToEnd();
                        Console.WriteLine(responseJson);
                    }
                }
                return responseJson;
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);                               
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
        static void Main(string[] args)
        {
              using (var repo = new Repository("D:\\IbaseIt\\Practice\\GitLibrary\\"))
              {
                var token = "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";                
                string branchName = "/feature/test-branch-csharp";
                const string accountName = "Anilkumar";
                const string userid = "anilkumar-ibaseit"; //---this is for github "anilkumar-ibaseit", "anilkumara"
                const string branchPrefix = "gitlibrary"; //---this is for github gitlibrary ,"testgitlib"
                branchName = branchPrefix + branchName;
                Remote remotebranch = repo.Network.Remotes[branchPrefix];


                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
               //var token22 = GetAccessToken();

               // return;
                // Ensure the branch is not the currently checked out branch
                if (repo.Head.FriendlyName == branchName)
                  {
                      // Checkout a different branch before setting up remote tracking
                      Branch masterBranch = repo.Branches["master"];
                      Commands.Checkout(repo, masterBranch);
                  }

                // Remove the branch if it already exists
                Branch branch = repo.Branches[branchName];
                if (branch != null)
                {
                    repo.Branches.Remove(branch);
                }

                // Create the branch
                branch = repo.CreateBranch(branchName) as Branch;


                // Commit and push changes
                var author = new Signature(accountName, userid, DateTimeOffset.Now);
                var committer = author;            
               
                repo.Merge(branch, repo.Config.BuildSignature(DateTimeOffset.Now));
                var changes = repo.Diff.Compare<TreeChanges>(branch.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory);
                Commands.Stage(repo, changes.Select(c => c.Path));
                Commit commit = repo.Commit("Commit changes from master to new branch", author, committer);
                repo.Branches.Update(branch,
                   b => b.Remote = repo.Network.Remotes[branchPrefix].Name,
                   b => b.UpstreamBranch = branch.CanonicalName);
                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, __, ___) =>
                        new UsernamePasswordCredentials { Username = token, Password = "" }
                };
                try
                {
                    repo.Network.Push(branch, pushOptions);
                }catch (Exception e)
                {
                    repo.Revert(commit, author);
                }
                // push to the server
              //  repo.Network.Push(repo.Head, pushOptions);
               // repo.Network.Push(branch, pushOptions);
                //  repopush.Network.Push(branch, options);

                if (branch != null)
                  {
                      Console.WriteLine("Branch created: " + branch.CanonicalName);
                    Console.ReadLine();
                  }
              }
            

/*
            var remoteUrl = "https://github.com/anilkumar-ibaseit/gitlibrary.git";
            var branchName = "master";
            var token = "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";

            // Open the cloned repository
            using (var repo = new Repository("D:\\IbaseIt\\Practice\\GitLibrary\\"))
            {
                // Commit and push changes
                var author = new Signature("Anilkumar", "anilkumar-ibaseit", DateTimeOffset.Now);
                var committer = author;
                var commit = repo.Commit("Commit message", author, committer);
                repo.Branches.Update(repo.Head,
                    b => b.Remote = repo.Network.Remotes["gitlibrary"].Name,
                    b => b.UpstreamBranch = repo.Head.CanonicalName);
                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, __, ___) =>
                        new UsernamePasswordCredentials { Username = token, Password = "" }
                };
                repo.Network.Push(repo.Head, pushOptions);
            }*/
        }
    }
}
