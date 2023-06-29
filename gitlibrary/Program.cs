using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace gitlibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
              using (var repo = new Repository("D:\\IbaseIt\\Practice\\GitLibrary\\"))
              {
                var token = "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";
                const string commitSha = "6f7d97f62da85093701559b0945fc322e0b5bbb9";
                  const string branchName = "gitlibrary/feature/test-branch-csharp";
                  Commit commit = repo.Lookup<Commit>(commitSha);

                // Push the branch to the remote repository
                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, __, ___) =>
                        new UsernamePasswordCredentials { Username = token, Password = "" }
                };
                Remote remotebranch = repo.Network.Remotes["gitlibrary"];

                  // Ensure the branch is not the currently checked out branch
                  if (repo.Head.FriendlyName == branchName)
                  {
                      // Checkout a different branch before setting up remote tracking
                      Branch masterBranch = repo.Branches["master"];
                      Commands.Checkout(repo, masterBranch);
                  }

                  // Remove the branch if it already exists
                  //Branch branch = repo.Branches[branchName];
                  //if (branch != null)
                  //{
                  //    repo.Branches.Remove(branch);
                  //}

                  // Create the branch
                  Branch branch = repo.CreateBranch(branchName) as Branch;


                  // Set up remote tracking for the branch               
                  Remote remote = repo.Network.Remotes["gitlibrary"];

                  // The local branch "buggy-3" will track a branch also named "buggy-3"
                  // in the repository pointed at by "origin"
                  if (remote != null)
                  {
                      repo.Branches.Update(branch,
                          b => b.Remote = remote.Name,
                          b => b.UpstreamBranch = branch.CanonicalName);
                  }


                  // Thus Push will know where to push this branch (eg. the remote)
                  // and which branch it should target in the target repository
                  // Disable SSL certificate verification
                  options.CertificateCheck = (_cert, _valid, _sslErrors) => true;
                  repo.Network.Push(branch, options);
                  //  repopush.Network.Push(branch, options);

                  if (branch != null)
                  {
                      Console.WriteLine("Branch created: " + branch.CanonicalName);
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
