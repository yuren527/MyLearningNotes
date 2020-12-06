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
```
As we know, a branch is no more than a named commit, so we can checkout to a specific commit without a branch name by `git checkout commit_hash>`, this is called `detached HEAD`, it is often used to recover a deleted branch, to do so we need to find out the commit by using `git -reflog` to filter the commit objects from thousands of objects

```
git diff //Compare the changes in working area with the staged files
git diff ---cached //Compare the staged files with the repository
```
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
