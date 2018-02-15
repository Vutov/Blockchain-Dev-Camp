pragma solidity ^0.4.18;

contract StructProblemContract {
    
    struct Account {
        string name;
        address addr;
        string email;
    }

    Account[] data;
    address owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function StructProblemContract() public {
        owner = msg.sender;
    }

    function add(string _name, string _email) public {
        Account memory account = Account({name : _name, addr: msg.sender, email: _email});
        data.push(account);
    }

    function get(uint index) isOwner public view returns (string name, string email) {
        Account memory dataForAddress = data[index];
        return (dataForAddress.name, dataForAddress.email);    
    }
}