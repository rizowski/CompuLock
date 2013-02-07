class CreateAccounts < ActiveRecord::Migration
  def change
    create_table :accounts do |t|
      t.references :computer
      
      t.string :domain, :null => false, :default => ""
      t.string :user_name, :null => false, :default => ""
      t.boolean :tracking, :null => false, :default => false
      t.time :allotted_time
      t.time :used_time

      t.timestamps
    end
  end
end
