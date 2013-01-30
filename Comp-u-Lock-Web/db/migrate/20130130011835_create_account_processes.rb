class CreateAccountProcesses < ActiveRecord::Migration
  def change
    create_table :account_processes do |t|
      t.references :account

      t.string :name
      t.datetime :lastrun
      
      t.timestamps
    end
  end
end
