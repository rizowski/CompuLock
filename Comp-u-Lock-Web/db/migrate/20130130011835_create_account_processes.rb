class CreateAccountProcesses < ActiveRecord::Migration
  def change
    create_table :account_processes do |t|
      t.references :account

      t.string :name, :null => false, :default => ""
      
      t.timestamps
    end
  end
end
