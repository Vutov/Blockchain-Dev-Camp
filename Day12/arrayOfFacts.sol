pragma solidity ^0.4.18;

contract ArrayOfFacts {

    string[] private facts;
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function ArrayOfFacts() public {
        owner = msg.sender;
    }

    function add(string fact) public isOwner {
        facts.push(fact);
    }

    function get(uint256 index) public view returns (string) {
        return facts[index];
    }

    function count() public view returns (uint256) {
        return facts.length;
    }
}