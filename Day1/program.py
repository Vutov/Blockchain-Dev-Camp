from flask import Flask, jsonify, request
from uuid import uuid4

from Day1.blockchain import Blockchain

app = Flask(__name__)
node_identifier = str(uuid4()).replace('-', '')

blockchain = Blockchain()

@app.route('/', methods={'GET'})
def root():
    return 'It\'s alive'

@app.route('/mine', methods={'Get'})
def mine():
    last_block = blockchain.last_block
    last_proof = last_block['proof']
    proof = blockchain.proof_of_work(last_proof)

    blockchain.new_transaction('0', node_identifier, 1)

    previous_hash = blockchain.hash(last_block)
    block = blockchain.new_block(proof, previous_hash)

    response = {
        'message': 'New Block Forged',
        'index': block['index'],
        'transactions': block['transactions'],
        'proof': block['proof'],
        'previous_hash': block['previous_hash']
    }

    return jsonify(response), 200

@app.route('/transactions/new', methods={'POST'})
def new_transaction():
    values = request.get_json()
    required = ['sender','recipient', 'amount']
    if not all (value in values for value in required):
        return 'Missing values', 400

    index = blockchain.new_transaction(values['sender'],values['recipient'], values['amount'])

    response = {'message': f'Transaction will be added to Block {index}'}
    return jsonify(response, 201)

@app.route('/chain', methods={'GET'})
def full_chain():
    response = {
        'chain': blockchain.chain,
        'length': len(blockchain.chain)
    }

    return jsonify(response), 200

@app.route('/nodes/register', methods={'POST'})
def register_nodes():
    values = request.get_json()
    nodes = values['nodes']
    if nodes is None:
        return 'Error: Please supply a valid list of nodes', 400

    for node in nodes:
        blockchain.register_node(node)

    response = {
        'message': 'New node have been added',
        'total_nodes': list(blockchain.nodes)
    }

    return jsonify(response), 201

@app.route('/nodes/resolve', methods={'GET'})
def consensus():
    replaced = blockchain.resolve_conflicts()

    message = 'Our chain is authoritative'
    if replaced:
        message = 'Our chain is replaced'

    response = {
        'message': message,
        'chain': blockchain.chain
    }

    return jsonify(response), 200

if __name__ == '__main__':
    from argparse import ArgumentParser

    parser = ArgumentParser()
    parser.add_argument('-p', '--port', default=5000, type=int, help='port to listen on')
    args = parser.parse_args()
    port = args.port

    app.run(host='127.0.0.1', port=port)

