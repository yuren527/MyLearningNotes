## Basic Console Commands ##
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

## Git Commands ##
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
git restore <file> //Restore the file to last commit
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
## Git low level commands ##
```
git cat-file <hash> //Hash is a hex string, in this case we don't have to paste in the entire hash code, but first 6 chars is enough, such as "8d0e41"
git cat-file -t <hash> //type
git cat-file -p <hash> //content
git cat-file -s <hash> //size
git ls-files
git ls-files -s
```
`git cat-file <hash>` is used to find the file from a hash object
## Other commands ##
This command is used to translate a content into a SHA1 code, but for git, all content are translated with a prefix attached, for example, "hello git" is actually translated from "blob 10\0hello git", so if we want get the actual hash of "hello git", we should do as `echo "blob 10\0hello git" | shasum`
```
echo "hello git" | shasum
```
## Core concept ##
- Git objects only store the contents of files, filenames are stored in index
- There is no folders in git, it is tree that organizes the files
