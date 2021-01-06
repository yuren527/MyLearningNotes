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
