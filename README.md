# Tech interviewing code examples

This repo contains sample code that can be used in tech interviews.

It currently contains two solutions, for two different use cases.

Both are built on .NET 7

## Person service with unit tests

### Contents

- A Person class
- A minimal PersonService stub that stores, gets, deletes and updates Person objects
- Unit tests to drive implementation of PersonService

### Suggested usage

This sample code is suitable for:
- when you are interviewing a relatively junior candidate
- when you want a warm-up task, e.g. when the candidate seems nervous - you can always start with this one and move on to the 2nd task
- when you want to see the candidate write code from scratch, to filter out candidates who talk better than they code

How to:
- Show the `Person` and `PersonService` classes.
- Show the test fixture, and present the names of the tests. Keep the test methods themselves collapsed to minimize distractions.
- Run all tests, show that they are all red.
- Expand the first test.
- Instruct the candidate to make the test green by only changing the PersonService class.
- Hand over the keyboard to the candidate. Ask them to talk out loud while coding.

### Suggested topics to explore with the candidate

- Class vs struct vs record
- Suitable collection types
- Unit testing in general

## Web scraper with recursion

### Contents

This is a sample solution for the take-home back end coding task that we already use for C# candidates - scrape the contents of a web site and store them to disk.

The code is based on a somewhat typical solution that we have received from candidates. It is intended to represent a basic but reasonable approach to solving the problem. The solution is based on a recursive approach, and while it uses async/await, it is not parallelized.

### Intentional shortcuts

The solution is simplified to make it easier to quickly overview and understand the code, within the time constraints of an interview. It ignores parts of the coding task and some best practices. Progress reporting and exception handling is minimal; URLs and paths are hard-coded as constants; there are no tests.

### Suggested usage

This sample code is suitable for
- when you are interviewing a relatively senior candidate
- when you want to discuss design, patterns, concurrency, performance

Send the description of the coding task to the candidate beforehand and ask them to think about how they would solve it, so that they will be familiar with the background.

Instruct them to focus on the coding part itself, not on supporting tasks (such as GDPR compliance, robots.txt and permissions to scrape, clarifying requirements, hosting, etc etc).

In the interview, present the solution to the candidate and walk them through the structure and high-level flow of code. Explain the intentional shortcuts that have been taken, so that you won't waste time on discussing those. (Unless you want to!)

### Code overview

The code:
1. Downloads and stores the page
1. Extracts and stores links to resources (images etc) for downloading later
1. Extracts links
1. Iterates through the links
    - Recurses to step 1
5. Downloads all resources

### Suggested topics to explore with the candidate

Some of these topics can be discussed in abstract; for others, you could have a stab at pair-programming the first steps.

- Potential risks with a recursive solution?
- How could we convert it into an iterative solution?
- How could we be more efficient and download pages in parallel?
- What might some suitable collection types be for parallel processing?