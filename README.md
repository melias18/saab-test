# Refactoring Test

## Assignment
You work within a team and are assigned a ticket created by the product owner as part of your sprint work.
The ticket says:

---

### Refactor Ticket Service
The ticket service functionally works well and the function and logic of it must not change. However it requires refactoring to improve readability. When refactoring, focus on applying clean code principles. The final result should be something ready to merge into the next release of the product and meets the definition of done. You are free to define the definition of done for yourself.
Keep in mind principles such as SOLID, KISS, DRY and YAGNI.

#### Limitations
Due to dependencies in other areas of the larger product, there are a few limitations that must be followed during the refactoring:

1. The contents of Program.cs cannot change at all including using statements.
2. The EmailService project cannot be changed
3. The method signatures in the repositories cannot change
4. The TicketRepository must remain static

---

Please submit your refactored code to us via a method of your choice (repository link, cloud share, zip file, etc.) prior to an interview to allow us to review it.
Note that while you can clone this repository, you cannot create a branch or commit any code to the repository. It is read only.

Try to limit time spent on this exercise to a maximum of 3 hours. If there is anything you don't have time to complete, write it down and we can discuss it during the interview.

Good Luck!


## Solutions
This section will cover the completed reafctory tasks and the comments I have on how I might be able to improve the code.

### Completed 
    [x] - Change the variable names to something descriptive.
    [x] - Simplify null check
    [x] - Create method to fetch of user
    [x] - Create method for determining priority 
    [x] - Create method to raise priority
    [x] - Create method to calculate price
    [x] - remove some self explanatory comments
    [x] - Check if title contains bad words using an array instead of using || statements

### Comments
    * All variable names need to be more descriptive. Same goes with methods.
    * The methods in original TicketService are too large and they are doing multiple tasks.
    * Many things in CreateTicket can be extracted into their own methods.
    * AssignTicket does two things. Gets user and assigns ticket (duplication). Getting the user is duplicated from CreateTicket. Other parts are simple enough to be kept.
    * Many parameters for CreateTicket. I would really like a absolute maximum of three if possible but this has been restricted. How would we go about refactoring that?
    * Is there anything else I can do other than extracting private methods? Can I rebuild the structure of Tickets? Maybe extract the private functions to its own class?
    * I would like to optimize the method of increasing the priority. Considering a substitute algorithm? 
    * Determine priority still does three things in its method and I would like to seperate them.
    * Refactor the enums to something more reusable that can hold its own method of increasing priority? Maybe something like this? https://ardalis.com/enum-alternatives-in-c/
    * Raise priority would break if any changes are made to the enums.
    * Logic of FetchUser should be handled in the background