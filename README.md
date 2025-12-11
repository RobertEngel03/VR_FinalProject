# VR_FinalProject — Setup Guide

This guide explains how new developers can set up the project locally and get ready to work.

---

## Prerequisites

- Git (latest stable version recommended)
- Git Bash / Terminal (You guys could also just get GitHub Desktop)
- Unity 2022.3.17f1

---

## First-Time Setup

1. **Clone the repository**:
```bash
git clone https://github.com/RobertEngel03/VR_FinalProject.git
cd VR_FinalProject
```
2. Install Unity and open the project with the correct editor version.
3. Check your repository status:
```bash
git status
```

## Configure Remote for Pushing and Pulling
By default, git clone sets the remote origin to the URL you cloned from.
To verify:
```bash
git remote -v
```
You should see something like:
```bash
origin  https://github.com/RobertEngel03/VR_FinalProject.git (fetch)
origin  https://github.com/RobertEngel03/VR_FinalProject.git (push)
```

## First Pull / Update
Before making any changes, always pull the latest code:
```bash
git checkout main
git pull origin main
```
## Pushing Changes
Stage your changes:
```bash
git add .
```
Commit with a message:
```bash
git commit -m "Describe your changes here"
```
Push to your branch (or main if permitted):
```bash
git push origin main
```
Note: The first time you push, Git will prompt for your GitHub username and password.
