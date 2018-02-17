pragma solidity ^0.4.18;

contract MainContract {
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function MainContract() public {
        owner = msg.sender;
    }

    function deposit() public payable {
        
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance;
    }
}

contract ToBeTerminated is MainContract {

    function terminate() isOwner public {
        selfdestruct(msg.sender);
    }
    
}