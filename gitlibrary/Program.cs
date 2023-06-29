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
                  const string branchName = "gitlibrary/feature/test-branch-csharp";
                
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


                // Commit and push changes
                var author = new Signature("Anilkumar", "anilkumar-ibaseit", DateTimeOffset.Now);
                var committer = author;            
               
                repo.Merge(branch, repo.Config.BuildSignature(DateTimeOffset.Now));
                var changes = repo.Diff.Compare<TreeChanges>(branch.Tip.Tree, DiffTargets.Index | DiffTargets.WorkingDirectory);
                Commands.Stage(repo, changes.Select(c => c.Path));
                Commit commit = repo.Commit("Commit changes from master to new branch", author, committer);
                repo.Branches.Update(branch,
                   b => b.Remote = repo.Network.Remotes["gitlibrary"].Name,
                   b => b.UpstreamBranch = branch.CanonicalName);
                var pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, __, ___) =>
                        new UsernamePasswordCredentials { Username = token, Password = "" }
                };
                repo.Network.Push(branch, pushOptions);
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
