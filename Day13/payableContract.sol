pragma solidity ^0.4.18;

contract PayableContract {
    
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function PayableContract() public payable {
        owner = msg.sender;
    } 

    function deposit() public payable {
        
    }

    // balance as function name hides this.balance
    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }
}