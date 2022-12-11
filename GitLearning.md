# Basic Console Commands #
```
clear
pwd
mkdir
ls
ls -a (git bash only)
rm <file>
rm -rf <file>
cd <path>
cd ..
more <file>
cat <file>
echo "<content>"><file>
touch <file>
tree
vim <file>
```

# Git Commands #
### Git Basic Commands ###
Show current version of git
```
git --version
```
Set and show config, a global config file will be created `C:\Users\用户名\.gitconfig`
```
git config --global user.name "yuren527"
git config --global user.email "yuren527@163.com"
git config user.name "yuren527"
git config user.email "yuren527@163.com"
git config -l
```
git repo commands
```
git init
git add
git status
git rm --cached <file>
git restore --staged <file> //Restore the staged status of the file, do not affect the file in working area
git restore <file> //Restore the file to last staged file
git commit -m"message"
```
git log commands
```
git log
git log --oneline
git log -2 //Show last 2 commits
git log --oneline -2
git log --after=='2020-05-21'
git log --before=='2020-=5-20'
git shortlog
git log --stat
git log --help
```
### Git Branch Commands ###
```
git branch
git branch <branch_name> //Create a branch based on current active branch, if there already have a branch with the given name, it will return a error
git branch -D <branch_name> //Force delete a branch, can't delete current active branch or branch not existing
git branch -d <branch_name> //Delete a branch with checking if the branch is fully merged into other branch
git branch --delete <branch_name>
git checkout <branch_name> //Change to a branch
git branch -m <old_name> <new_name> //Rename the branch, only rename the file of head
git -reflog
git checkout -b <branch_name> //Create a new branch and checkout to it
git checkout <file> //Restore a file using checkout from the repository
git merge <branch_name>
git rebase <branch_name> //Reabse the current active branch to the latest commit of given branch
git push origin <branch_name> --force
```
As we know, a branch is no more than a named commit, so we can checkout to a specific commit without a branch name by `git checkout commit_hash>`, this is called `detached HEAD`, it is often used to recover a deleted branch, to do so we need to find out the commit by using `git -reflog` to filter the commit objects from thousands of objects
```
git diff //Compare the changes in working area with the staged files
git diff ---cached //Compare the staged files with the repository
```
### Git Remote Branch Commands ###
**Branch creation commands:**

`git push -u origin <branch_name>` Push local branch to remote branch, if <branch_name> branch does not exist remotely, it would be created. 

**Branch deletion:**

`git branch -d <branch_name>` delete local branch 

`git push origin -d <branch_name>` delete remote branch 

**List all local and remote branch:**

`git branch -a` list all branches both local and remote 

`git branch -r` list all remote origin branches 

`git branch -vv` list all local branches tracking information 

**Sync remote branch:**

`git fetch` download objects and refs from remote repository `--prune` Before fetching, remove any remote-tracking references that no longer exist on the remote
`git pull` fetch and merge origin to local tracking branch

### Git Remote Commands ###
`git remote add/remove <name> <url>` add a remote 

`git remote` get remote name, origin be default, `-v` display more information like remote url 

`git remote show origin` show remote origin tracking information, need connect to internet 

`git remote prune origin` delete local origin branch which does not exist on remote

# Git Low Level Commands #
```
git cat-file <hash> //Hash is a hex string, in this case we don't have to paste in the entire hash code, but first 6 chars is enough, such as "8d0e41"
git cat-file -t <hash> //type
git cat-file -p <hash> //content
git cat-file -s <hash> //size
git ls-files
git ls-files -s
```
`git cat-file <hash>` is used to find the file from a hash object
# Other Commands #
This command is used to translate a content into a SHA1 code, but for git, all content are translated with a prefix attached, for example, "hello git" is actually translated from "blob 10\0hello git", so if we want get the actual hash of "hello git", we should do as `echo "blob 10\0hello git" | shasum`
```
echo "hello git" | shasum
```
# Core Concept #
- Git objects only store the contents of files, filenames are stored in index
- There is no folders in git, it is tree that organizes the files
- Deleting a branch does not delete any objects, but delete head in `logs/refs/heads/` and `refs/heads/` only
- Nobldy should work on master branch directly, everyone have his own working branch, when his job is done, try to merge his branch to the master branch
- After merging a branch, a file named as `ORIG_HEAD` pointing to the last commit before merging is generated under .git, this is used to restore from merging in case of promblems caused by merging, merging operation is sometimes risky, use `git reset ORIG_HEAD` to restore
- Git rebase should never be used on master branch and branches which are used by other developers, as rebase has to be force pushed to remote repository and overwrite the hash of all commits of the branch, it could cause many serious problems for others wo pulled the branch from remote repository
- `git pull` equals to a 2 steps operation, which fetch from remote to async `origin/master` first, then merge `origin/master` to `master`. There is two possible  situation, one is fast forward, another is 3 way merge. If it is a 3-way-merge, the `origin/master` is altered to pointing to a new commit after 3-way-merge, meanwhile, the remote master branch is still pointing to the old commit, so we should push it to remote again to make it sync

# Access Github Using SSH Key #
1. Generate a openSSH keypair with the command `ssh-keygen -t ed25519 -C "<github_email>"` in git bash, a file name is required and it will be "id_ed25519" if left blank
2. Copy and paste the public key to github's ssh access settings
3. Put the private key into the `<username>/.ssh/` folder
4. Use `eval "$(ssh-agent -s)"` to see if the ssh agent is running in the background
5. Use `ssh-add ~/.ssh/<private_key_filename>` to add the key to the agent, or add `IdentityFile ~/.ssh/<private/key/filename>` to `C:\Program Files\Git\etc\ssh\ssh_config`
6. Test the connection to server with `ssh -T git@github。com`
7. Clone the repository with `git clone --branch <branch_name> --depth 1 <github ssh address>` in git bash

# Clone Several Specific Branches With Depth 1 # 
Take UnrealEngine Repository as an example, clone the branch 5.1 and branch 4.27 to local with depth 1.
1. Use the command `$ git clone git@github.com:EpicGames/UnrealEngine.git --branch 4.27 --depth 1` in GitBash to clone the specific branch with depth 1, this will dramatically reduce cloning time. After it finishes, the branch 4.27 is cloned successfully. 
2. There may be some bugs that, if we directly fetch branch 5.1 now, the remote branch 5.1 will not show up when we use the command `git branch -r` to see the list of remote branches, and if input `git config --local -l`, we will find out that `remote.origin.fetch=+refs/heads/4.27:refs/remotes/origin/4.27` is in the config which is not supposed to be. To fix this, we need to delete the remote `origin` and re-add it to the remotes using `git remote remove origin` and `git remote add github git@github.com:EpicGames/UnrealEngine.git`(renaming remote to github meanwhile). And in the config we can see `remote.github.fetch=+refs/heads/*:refs/remotes/github/*` which is expected now.
3. Again fetch the branch 4.27 and 5.1 by inputing `git fetch github 4.27 --depth 1` and `git fetch github 5.1 --depth 1`, we can see remote branches are correctly added to the remote branch list now(`git branch -r`).
4. After fetching branch 5.1 from github, input `git branch 5.1 fetch_head` to create a new branch from the fetch_head.
5. Last step, connect local branches to the remote branches by inputing `git branch --set-upstream-to=github/5.1` and the same to branch 4.27.
