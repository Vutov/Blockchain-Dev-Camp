pragma solidity ^0.4.18;

contract CallableDeposit {
      
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function CallableDeposit() public {
        owner = msg.sender;
    }

    function deposit() public payable {
        
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }

    function sendBalance(address addr) public {
        selfdestruct(addr);
    }
}

contract NoPayable {

    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function NoPayable() public {
        owner = msg.sender;
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }
}