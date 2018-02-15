pragma solidity ^0.4.18;

contract RegistryOfCertificates {
    
    mapping (string=>uint) private certificates;
    address private owner;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function RegistryOfCertificates() public {
        owner = msg.sender;
    }

    function add(string certHash) public isOwner {
        certificates[certHash] = now;
    }

    function exists(string certHash) public view returns(bool present, uint timestamp) {
        return (certificates[certHash] != 0, certificates[certHash]);
    }
}