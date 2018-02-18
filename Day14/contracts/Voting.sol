// 5. Create a public field, which for each candidate stores his votes.
// 6. Create a private array storing every candidate.
// 7. Create addCandidate(bytes32) function, which adds the candidate to an array.
// 8. Create validCandidate(bytes32) function, which checks whether the given candidate is contained in the array.
// 9. Then create function voteForCandidate(bytes32), which votes for given candidate.
// 10. Also, you will need to create function totalVotesFor(bytes32) which will return the count of votes for a
// candidate.

pragma solidity ^0.4.18;

contract VotingContract {
    mapping (bytes32=>uint) private candidateVotes;
    bytes32[] private candidates;
    mapping (address=>bool) private voters;

    function addCandidate(bytes32 candidate) public {
        require(candidateVotes[candidate] == 0);
        require(voters[msg.sender] == false);

        vote(candidate);
    }

    function validCandidate(bytes32 candidate) public view returns(bool) {
        return candidateVotes[candidate] > 0;
    }

    function voteForCandidate(bytes32 candidate) public {
        require(candidateVotes[candidate] > 0);
        require(voters[msg.sender] == false);

        vote(candidate);
    }

    function totalVotesFor(bytes32 candidate) public view returns(uint) {
        require(candidateVotes[candidate] > 0);
        return candidateVotes[candidate];
    }

    function vote(bytes32 candidate) private {
        candidateVotes[candidate]++;
        candidates.push(candidate);
        voters[msg.sender] = true;
    }
}