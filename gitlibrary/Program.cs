using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace gitlibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var repo = new Repository("D:\\IbaseIt\\Practice\\GitLibrary\\"))
            {
                const string commitSha = "c05c5677a9c98b80273a8ccb846a904d9b90d877";
                const string branchName = "feature/test-branch-csharp";
                Commit commit = repo.Lookup<Commit>(commitSha);
                repo.Branches.Remove(branchName);
                Branch branch = null;
                if (!repo.Branches.Equals(new[] { commit }))
                {
                    branch = repo.Branches.Add(branchName, commit);
                }
                repo.Branches.Update(branch, b => b.Remote = "origin", b => b.UpstreamBranch = branch.CanonicalName);

                PushOptions options = new PushOptions
                {
                    CredentialsProvider = (url, _user, _cred) =>
                        new UsernamePasswordCredentials
                        {
                            Username = "anilkumar.ibaseit@hotmail.com",
                            Password = "Anil1@34"
                        }
                };
                repo.Network.Push(branch, options);
                if (branch != null)
                {
                    Console.WriteLine("Branch created: " + branch.CanonicalName);
                }
                //foreach (Branch b in ListBranchesContainingCommit(repo, commitSha))
                //{
                //    Console.WriteLine(b.CanonicalName);
                //}
            }
        }
    }
}
