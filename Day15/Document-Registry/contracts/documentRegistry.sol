pragma solidity ^0.4.18;

contract DocumentRegistry {
    address private owner;
    mapping (string=>uint) documents;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function DocumentRegistry() public {
        owner = msg.sender;
    }

    function add(string hash) isOwner public returns(uint dateCreated) {
        documents[hash] = block.timestamp;
        return block.timestamp;
    }

    function vefiry(string hash) public view returns(uint dateCreated) {
       return documents[hash];
    }
}