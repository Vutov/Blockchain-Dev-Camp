pragma solidity ^0.4.18;

contract FallbackExercise {
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function FallbackExercise() public {
        owner = msg.sender;
    }

    function deposit() public payable {
        
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }

    function sendBalance(address addr, uint amount) isOwner public {
        require(this.balance > amount);
        addr.transfer(amount);
    }
}

contract RecipientContract {
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function RecipientContract() public {
        owner = msg.sender;
    }

    function() public payable {
        
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }
}