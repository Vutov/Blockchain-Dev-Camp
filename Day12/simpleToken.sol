pragma solidity ^0.4.18;

contract SimpleTokenContract {
    
    mapping (address=>uint) private tokens;
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function SimpleTokenContract() public {
        owner = msg.sender;
        tokens[msg.sender] = 100;
    }

    function transfer(address addr, uint amount) public isOwner {
        // tokens[msg.sender] - amount >= 0 not working because of uints :)
        require(tokens[msg.sender] >= amount);
        require(tokens[addr] + amount > tokens[addr]);

        tokens[msg.sender] -= amount;
        tokens[addr] += amount;
    }

    function check(address addr) public view returns (uint) {
        return tokens[addr];
    }
}