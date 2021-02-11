@echo off
call hugo
choice /d y /t 3 > nul
git config core.autocrlf true
git add .
git commit -m "build"
git push https://%PERSONAL_GIT%@github.com/eyalmolad/gotask.git


