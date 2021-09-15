# The Beta Tester's Guide to Gravity Manipulation

You've signed up to volunteer as a beta tester for Gravitronix Industries' mysterious new technology. Explore their facilities and solve puzzles as you learn to manipulate the force of gravity itself.

## Setup

Programmers and Level Designers will need access to this Github repository and to Unity.

This project is running on Unity version 2020.3.8f1. You can use any more recent subversion of 2020.3 to work on it though. Use [Unity Hub](https://unity3d.com/get-unity/download) to install a usable version of Unity if you don't already have it.

To set up the repository, first Fork MGPAlpha/GravityGuide on Github and clone the forked repo to your local machine. You will also need to add MGPAlpha/GravityGuide as the upstream repo in order to be able to pull new changes. To do this:

```bash
git remote add upstream https://github.com/MGPAlpha/GravityGuide.git
```

Or if you're using Github over SSH use:

```bash
git remote add upstream git@github.com:MGPAlpha/GravityGuide.git
```

From that point, the basic workflow for adding new content to the repo will be:

1. Checkout the local `main` branch: `git checkout main`
1. Pull any updates from the upstream repository: `git pull upstream main`
1. Checkout a new branch for your feature: `git checkout -b feat/your-feature-name`

Once you've made your changes, like adding a new mechanic or building a new level:

1. Add all the files you changed to staging: `git add Assets/changedfile1 Assets/changedfile2 ...`
    * Note that your version of `ProjectSettings/ProjectVersion.txt` will have changes if you are using a newer release of Unity, so don't add that file, or any file that isn't in `Assets/` unless you know what you're doing.
1. Write the staged changes to a new commit: `git commit -m "Describe your changes here"`
1. Push the changes to a new branch on your forked repository: `git push -u feat/your-feature-name`
1. Go to your forked repo on Github and make a pull request to merge your changes into MGPAlpha/GravityGuide:main

I will then review the changes and accept the pull request, which will merge your changes into the repository. The next time anyone pulls from upstream, they'll have your changes.

## Documentation

I don't currently have a lot of documentation done on how to use the various objects I've made for level design, but I plan to have readmes available for each major design element (doors, sensors, gravity objects, etc.) available in the [Docs](Docs/) folder.